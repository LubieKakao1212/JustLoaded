using Aimless.ModLoader.Util.Algorithm;
using Aimless.ModLoader.Util.Extensions;

namespace Aimless.ModLoader.Util.Tests;

public class TopoSortTests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Identity()
    {
        var toSort = new HashSet<int>(Enumerable.Range(0, 10));
        var dependencies = Enumerable.Empty<(int, int)>();
        
        var sorter = CreateSorter(toSort, dependencies);

        foreach (var element in sorter.Sort())
        {
            toSort.Remove(element);
        }

        Assert.That(toSort, Has.Count.EqualTo(0));
    }

    [Test]
    public void Sort()
    {
        var toSort = new HashSet<int>(Enumerable.Range(0, 10));
        var dependencies = new[] {
            (1, 0),
            (1, 2),
            (3, 0),
            (9, 7),
        };
        
        var sorter = CreateSorter(toSort, dependencies);

        AssertSorted(sorter.Sort(), dependencies);
    }
    
    [Test]
    public void Cycle() {
        var toSort = new HashSet<int>(Enumerable.Range(0, 10));
        var dependencies = new[] {
            (0, 1),
            (1, 2),
            (2, 0),
        };
        
        var sorter = CreateSorter(toSort, dependencies);
        Assert.Throws<ApplicationException>(() => sorter.Sort());
    }
    
    [Test]
    public void NoElementFirst() {
        var toSort = new HashSet<int>(Enumerable.Range(0, 10));
        var dependencies = new [] {
            (0, 10)
        };
        
        Assert.Throws<ApplicationException>(() => CreateSorter(toSort, dependencies));
    }

    [Test]
    public void NoElementSecond() {
        var toSort = new HashSet<int>(Enumerable.Range(0, 10));
        var dependencies = new [] {
            (10, 0)
        };
        
        Assert.Throws<ApplicationException>(() => CreateSorter(toSort, dependencies));
    }

    
    [Test]
    public void ResetDeps()
    {
        var toSort = new HashSet<int>(Enumerable.Range(0, 10));
        
        var dependencies1 = new[] {
            (0, 1),
            (1, 2),
            (2, 0),
        };
        
        var dependencies2 = new[] {
            (1, 0),
            (1, 2),
            (3, 0),
            (9, 7),
        };
        
        var sorter = CreateSorter(toSort, dependencies1);
        sorter.ClearDependencies();
        AddDeps(sorter, dependencies2);
        
        AssertSorted(sorter.Sort(), dependencies2);
    }
    
    private Dictionary<T, HashSet<T>> CreateGraph<T>(IEnumerable<T> elements, IEnumerable<(T source, T dependency)> dependencies) where T : notnull
    {
        var dictOut = new Dictionary<T, HashSet<T>>();

        foreach (var element in elements)
        {
            dictOut.Add(element, new());
        }

        foreach (var dep in dependencies)
        {
            dictOut.AddNested(dep.source, dep.dependency);
        }

        return dictOut;
    }

    private TopoSorter<T> CreateSorter<T>(IEnumerable<T> elements, IEnumerable<(T source, T dependency)> dependencies) where T : notnull {
        var sorter = new TopoSorter<T>();
        foreach (var element in elements) {
            sorter.AddElement(element);
        }

        foreach (var dep in dependencies) {
            sorter.AddDependency(dep.source, dep.dependency);
        }

        return sorter;
    }

    private TopoSorter<T> AddDeps<T>(TopoSorter<T> sorter, IEnumerable<(T source, T dependency)> dependencies) {
        foreach (var dep in dependencies)
        {
            sorter.AddDependency(dep.source, dep.dependency);
        }

        return sorter;
    }
    
    private void AssertSorted<T>(IEnumerable<T> sorted, IEnumerable<(T source, T dependency)> dependencies) {
        var enumerable = sorted as T[] ?? sorted.ToArray();
        var graph = CreateGraph(enumerable, dependencies);
        var encountered = new HashSet<T>();
        
        foreach (var element in enumerable) {
            foreach (var dep in graph[element]) {
                Assert.That(encountered, Contains.Item(dep));
            }
            encountered.Add(element);
        }
    }
    
}