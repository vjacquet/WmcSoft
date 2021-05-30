# TODO

## Algorithms

- [ ] Implement Peaks algorithm on an IEnumerable<T> 
  see https://stackoverflow.com/questions/5269000/finding-local-maxima-over-a-dynamic-range   
  https://www.filipekberg.se/2014/02/10/understanding-peak-finding/  
  https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=&ved=2ahUKEwjcsdns_7nwAhUj8-AKHdLmDcQQFjAAegQIBBAD&url=https%3A%2F%2Fciteseerx.ist.psu.edu%2Fviewdoc%2Fdownload%3Fdoi%3D10.1.1.37.1110%26rep%3Drep1%26type%3Dpdf&usg=AOvVaw0vLSGtjpoB_b88qRKB9e9B  
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
