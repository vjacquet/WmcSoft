# TODO

## Algorithms

- [ ] Implement Peaks algorithm on an IEnumerable<T>
- [ ] Implement HaskPeak algorithm on an IEnumerable<T>

## Business

- [ ] Implement `BusinessCalendar.Enumerator` to enumerate the business days of a Business calendar.
- [ ] Change the signature of `IBusinessCalendar` to specialize `IEnumerable<Date>`. 
- [x] Implement `Until`, `Since` and `Between` extensions to truncate a business calendar.

## Refactoring

- [x] Rename `UpgradingFileStreamSource` to `CachedFileStreamSource`.

## Optimizations

- [x] Optimize `HeaderedTextWriter` by overriding more method to give a chance of specialized methods on the underlying `TextWriter`.

## Tools

- [x] Consolidate nuget packages
- [x] Upgrade nuget packages
- [ ] Upgrade nuget packages for WmcSoft.VisualStudio
