<h1 align="center">
<img src="https://raw.githubusercontent.com/coenm/ImageHash/master/icon/ImageHash.512.png" alt="ImageHash" width="256"/>
<br/>
ImageHash
</h1>
<div align="center">

[![Build status](https://ci.appveyor.com/api/projects/status/891pi650ra2ori5t/branch/develop?svg=true)](https://ci.appveyor.com/project/coenm/imagehash/branch/develop) [![Build Status](https://travis-ci.org/coenm/ImageHash.svg?branch=develop)](https://travis-ci.org/coenm/ImageHash) [![Coverage](https://codecov.io/gh/coenm/imagehash/branch/develop/graph/badge.svg)](https://codecov.io/gh/coenm/imagehash) [![NuGet](https://img.shields.io/nuget/v/CoenM.ImageSharp.ImageHash.svg)](https://www.nuget.org/packages/CoenM.ImageSharp.ImageHash/) [![MyGet Pre Release](https://img.shields.io/myget/coenm/vpre/CoenM.ImageSharp.ImageHash.svg?label=myget)](https://www.myget.org/feed/Packages/coenm/)

</div>
.NET Standard library containing multiple algorithms to calculate perceptual hashes of images and to calculate similarity using those hashes.


## Perceptual hash
Definition by [phash.org](https://www.phash.org/)

> A perceptual hash is a fingerprint of a multimedia file derived from various features from its content. Unlike cryptographic hash functions which rely on the avalanche effect of small changes in input leading to drastic changes in the output, perceptual hashes are "close" to one another if the features are similar.

## Hash Algorithms
This project implements the following algorithms:
- **AverageHash** by Dr. Neal Krawetz. Check his blog/article [here](http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html);
- **PerceptualHash** also by Dr. Neal Krawetz.
- **DifferenceHash** by David Oftedal with minor improvements of Dr. Neal Krawetz. More information can be found [here](http://01101001.net/programming.php) or [here](http://www.hackerfactor.com/blog/index.php?/archives/529-Kind-of-Like-That.html).

## Credits
- This project uses the [ImageSharp](https://sixlabors.com/projects/imagesharp/) library for image processing;
- Also based on existing implementations of already listed algorithms: [jenssegers/imagehash](https://github.com/jenssegers/imagehash) and [jforshee/ImageHashing](https://github.com/jforshee/ImageHashing);
- Icon created by Rik.

## API

### Calculate image hash
```csharp
var hashAlgorithm = new AverageHash();
// or one of the other available algorihms:
// var hashAlgorithm = new DifferenceHash();
// var hashAlgorithm = new PerceptualHash();

string filename = "your filename";
using (var stream = File.OpenRead(filename))
{
    ulong imageHash = hashAlgorithm.Hash(stream);  
}
```

### Calculate image similarity
Note that to calculate the image similarity, both image hashes should have been calculated using the same hash algorihm.

```csharp
// calculate the two image hashes
ulong hash1 = hashAlgorithm.Hash(imageStream1);  
ulong hash2 = hashAlgorithm.Hash(imageStream2);  

double percentageImageSimilarity = CompareHash.Similarity(hash1, hash2);
```

You can also take a look at [DotNet APIs](http://dotnetapis.com/pkg/CoenM.ImageSharp.ImageHash).


