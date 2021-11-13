# ImageHash

Perceptual image hashing in netstandard using the ImageSharp library. Includes three hashing algorithms (AverageHash, DifferenceHash, and PerceptualHash).

## Calculate image hash

```cs
var hashAlgorithm = new AverageHash();
// or one of the other available algorithms:
// var hashAlgorithm = new DifferenceHash();
// var hashAlgorithm = new PerceptualHash();

string filename = "your filename";
using var stream = File.OpenRead(filename);

ulong imageHash = hashAlgorithm.Hash(stream);
```

## Calculate image similarity
Note that to calculate the image similarity, both image hashes should have been calculated using the same hash algorihm.

```cs
// calculate the two image hashes
ulong hash1 = hashAlgorithm.Hash(imageStream1);
ulong hash2 = hashAlgorithm.Hash(imageStream2);

double percentageImageSimilarity = CompareHash.Similarity(hash1, hash2);
```
