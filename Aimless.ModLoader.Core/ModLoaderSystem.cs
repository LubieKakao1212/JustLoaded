using Aimless.ModLoader.Content;
using Aimless.ModLoader.Content.Database;
using Aimless.ModLoader.Core.Discovery;
using Aimless.ModLoader.Core.Loading;
using Aimless.ModLoader.Util;
using Aimless.ModLoader.Util.Algorithm;
using Aimless.ModLoader.Util.Extensions;

namespace Aimless.ModLoader.Core;

public class ModLoaderSystem {
    public InitializationPhase CurrentInitPhase { get; private set; }
    public IReadOnlyMasterDatabase MasterDb => _masterDb;
    
    private readonly IModProvider _modProvider;
    private readonly IModFilter _modFilter;
    private readonly MasterDatabase _masterDb;

    private readonly ArrayDatabase<ILoadingPhase> _loadingPhases = new();
    private readonly ArrayDatabase<Mod> _mods = new();
    private readonly HashSet<Mod> _modsSet = new();
    
    private ModLoaderSystem(IModProvider modProvider, IModFilter modFilter, MasterDatabase masterDb) {
        this._masterDb = masterDb;
        this._modProvider = modProvider;
        this._modFilter = modFilter;
        RegisterDefault();
        this.CurrentInitPhase = InitializationPhase.Created;
    }

    public void DiscoverMods() {
        CurrentInitPhase.AssertBefore(InitializationPhase.DiscoveringMods);
        CurrentInitPhase = InitializationPhase.DiscoveringMods;
        SetMods(
            _modFilter.FilterMods(
                new HashSet<Mod>(
                    _modProvider.DiscoverMods()
                    )
                )
            );
    }

    /// <summary>
    /// Used To manually set mods to be loaded, can only be called once<br/>
    /// If you use this DON'T use <see cref="DiscoverMods"/>
    /// </summary>
    public void SetMods(HashSet<Mod> mods) {
        if (_modsSet.Count > 0) {
            CurrentInitPhase = InitializationPhase.ErroredInvalidState;
        }
        CurrentInitPhase.AssertBefore(InitializationPhase.ModsSet);
        
        _modsSet.UnionWith(mods);
        CurrentInitPhase = InitializationPhase.ModsSet;
    }

    /// <summary>
    /// Resolves dependencies between mods and fills database core:mods
    /// </summary>
    public void ResolveDependencies() {
        CurrentInitPhase.AssertAt(InitializationPhase.ModsSet);
        
        var modKeys = new HashSet<ContentKey>();
        var modsByKey = new Dictionary<ContentKey, Mod>();
        #region Get Keys and handle duplicates

        var duplicates = new Dictionary<ContentKey, int>();
        foreach (var mod in _modsSet) {
            var key = mod.Metadata.ModKey;
            if (modKeys.Contains(key)) {
                duplicates.PreIncrement(key);
            }

            modKeys.Add(key);
        }
        foreach (var duplicate in duplicates) {
            //TODO use logger (Error)
            Console.Error.WriteLine($"Mod with id {ModMetadata.ToModId(duplicate.Key)} was found {duplicate.Value + 1} times");
            CurrentInitPhase = InitializationPhase.ErroredDuplicateMods;
        }
        if (!IsOk()) {
            return;
        }

        foreach (var mod in _modsSet) {
            modsByKey.Add(mod.Metadata.ModKey, mod);
        }
        #endregion
        
        #region Validate Required

        var failedDependencies = new HashSet<(ContentKey mod, ContentKey dependency)>();
        foreach (var mod in _modsSet) {
            var meta = mod.Metadata;
            foreach (var dep in meta.HardDependencies.Keys) {
                if (!modKeys.Contains(dep)) {
                    failedDependencies.Add((meta.ModKey, dep));
                }
            }
        }
        foreach (var failed in failedDependencies) {
            //TODO use logger (Error)
            Console.Error.WriteLine($"Missing dependency { failed.dependency } is required by { failed.mod }");
            CurrentInitPhase = InitializationPhase.ErroredMissingModDependencies;
        }
        if (!IsOk()) {
            return;
        }
        #endregion

        #region Sort

        var sorter = new TopoSorter<ContentKey>();
        foreach (var key in modKeys) {
            sorter.AddElement(key);
        }
        
        //Required
        foreach (var mod in _modsSet) {
            var meta = mod.Metadata;
            foreach (var dep in meta.HardDependencies) {

                switch (dep.Value) {
                    case Order.After:
                        sorter.AddDependency(meta.ModKey, dep.Key);
                        continue;
                    case Order.Before:
                        sorter.AddDependency(dep.Key, meta.ModKey);
                        continue;
                    default:
                        continue;
                }
            }
        }
        //Optional
        foreach (var mod in _modsSet) {
            var meta = mod.Metadata;
            foreach (var dep in meta.SoftDependencies) {
                if (dep.Value == Order.Any) {
                    //TODO use logger
                    Console.Out.WriteLine($"Optional dependency with { Order.Any } found for { meta.ModKey } on { dep }, this does nothing and be skipped");
                    continue;
                }
                if (!modKeys.Contains(dep.Key)) {
                    continue;
                }
                
                switch (dep.Value) {
                    case Order.After:
                        sorter.AddDependency(meta.ModKey, dep.Key);
                        continue;
                    case Order.Before:
                        sorter.AddDependency(dep.Key, meta.ModKey);
                        continue;
                    default:
                        continue;
                }
            }
        }
        
        _mods.Init(sorter.Sort().Select((key) => new KeyValuePair<ContentKey, Mod>(key, modsByKey[key])));
        #endregion

        CurrentInitPhase = InitializationPhase.DependenciesResolved;
    }

    /// <summary>
    /// Calls <see cref="IModInitializer.SystemInit"/> for every mod in order <br/>
    /// Needs to be called after <see cref="ResolveDependencies"/>
    /// </summary>
    public void InitMods() {
        CurrentInitPhase.AssertAt(InitializationPhase.DependenciesResolved);
        CurrentInitPhase = InitializationPhase.ModSystemInitializing;
        
        var resolver = new OrderedResolver<ILoadingPhase>();
        try {
            foreach (var mod in _mods.ContentValues) {
                mod.Initializer.SystemInit(mod, resolver);
            }
        }
        catch (Exception) {
            CurrentInitPhase = InitializationPhase.ErroredGeneric;
            throw;
        }
        

        _loadingPhases.Init(resolver.Resolve());
        CurrentInitPhase = InitializationPhase.ModSystemInitialized;
    }

    /// <summary>
    /// Executes registered <see cref="ILoadingPhase"/>s <br/>
    /// Needs to be called after <see cref="InitMods"/>
    /// </summary>
    public void Load() {
        CurrentInitPhase.AssertAt(InitializationPhase.ModSystemInitialized);
        CurrentInitPhase = InitializationPhase.Loading;

        try {
            foreach (var phase in _loadingPhases.ContentValues) {
                phase.Load(this);
            }
        }
        catch (Exception) {
            CurrentInitPhase = InitializationPhase.ErroredGeneric;
            throw;
        }

        CurrentInitPhase = InitializationPhase.Ready;
    }
    
    private void RegisterDefault() {
        _masterDb.RegisterDatabase(new ContentKey("core:loading-phases"), _loadingPhases, DBRegistrationType.Main);
        _masterDb.RegisterDatabase(new ContentKey("core:mods"), _mods, DBRegistrationType.Main);
    }

    private bool IsOk(bool log = true) {
        if (CurrentInitPhase.IsErrored()) {
            if (log) {
                //TODO use logger (Error)
                Console.Error.WriteLine($"Startup May not continue due to errors");
            }
            return false;
        }

        return true;
    }
    
    public class Builder {

        private readonly IModProvider _modProvider;
        public MasterDatabase? MasterDb { get; init; }
        public IModFilter? ModFilter { get; init; }

        public Builder(IModProvider modProvider) {
            this._modProvider = modProvider;
        }

        public ModLoaderSystem Build() {
            var db = MasterDb ?? new MasterDatabase();
            var filter = ModFilter ?? IdentityModFilter.Instance;
            return new ModLoaderSystem(_modProvider, filter, db);
        }
        
    }
}