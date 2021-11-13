```cs
var hashAlgorithm = new AverageHash();
// or one of the other available algorithms:
// var hashAlgorithm = new DifferenceHash();
// var hashAlgorithm = new PerceptualHash();

string filename = "your filename";
using var stream = File.OpenRead(filename);

ulong imageHash = hashAlgorithm.Hash(stream);
```
