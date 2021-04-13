# PLists
![Nuget](https://img.shields.io/nuget/v/PLists)
![.NET](https://github.com/carlopantaleo/PLists/workflows/.NET/badge.svg)

A prototype-based extensible property list.


## Background
I was reading the excellent article by Steve Yegge 
"[The Universal Design Pattern](https://web.archive.org/web/20210221093721/http://steve-yegge.blogspot.com/2008/10/universal-design-pattern.html)".
I also needed a simple yet effective implementation of a property list which adhered to the 
"properties pattern", in order to help me build a configuration system which allowed to easily
follow the concept of "convention over configuration".

I ended up building my own property list as an implementation of the `IDictionary` interface.


## Usage
Creating and manipulating a basic property list is very straight-forward, as it's actually as
operating on a `Dictionary`.

```c#
var pList = new PList<string, string> {
    ["prop1"] = "value1",
    ["prop2"] = "value2"
};

Assert.Equal("value1", pList["prop1"]);
Assert.Throws<PropertyNotFoundException<string>>(() => pList["propx"]);
Assert.True(pList.TryGetValue("prop1", out _));
Assert.False(pList.TryGetValue("propx", out _));
```

The power of `PList` comes in its ability to inherit from other `PList`s.

```c#
// Create another PList which inherits from the previously defined pList.
var extPlist = new PList<string, string>(pList) {
    ["prop1"] = "overriden",
    ["propx"] = "valuex"
};

// "prop1" overrides the same property defined in pList.
Assert.Equal("overriden", extPlist["prop1"]);
Assert.Equal("value1", pList["prop1"]);

// "propx" is defined only in the inheriting PList.
Assert.Equal("valuex", extPlist["propx"]);
Assert.False(pList.TryGetValue("propx", out _));

// "prop2" is inherited from pList.
Assert.Equal("value2", extPlist["prop2"]);

// A inherited property may be removed from the inheriting PList: the inheritance link breaks.
extPlist.Remove("prop2");
Assert.False(extPlist.TryGetValue("prop2", out _));
Assert.True(pList.TryGetValue("prop2", out _));

// A inherited property may be removed from the base PList: it won't be available in any inheriting PList.
pList.Remove("prop1");
Assert.False(extPlist.TryGetValue("prop1", out _));
Assert.False(pList.TryGetValue("prop1", out _));
```


## Extensions
`PList` is an in-memory property list. If persistence or any other more advanced usages are
needed, you are free to extend it.

The class `PList` is `sealed`, so in order to extend it I strongly recommend you to use the
"decorator pattern" by implementing the interface `IPlist`. Actually, `PList` itself extends
`Dictionary` through that pattern, so you are encouraged to do the same 😊.