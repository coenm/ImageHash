# ImageHash

| Branch | Status |
| :--- | :--- |
| Develop | [![Build status](https://ci.appveyor.com/api/projects/status/891pi650ra2ori5t/branch/develop?svg=true)](https://ci.appveyor.com/project/coenm/imagehash/branch/develop) [![Build Status](https://travis-ci.org/coenm/ImageHash.svg?branch=develop)](https://travis-ci.org/coenm/ImageHash) [![Coverage](https://codecov.io/gh/coenm/imagehash/branch/develop/graph/badge.svg)](https://codecov.io/gh/coenm/imagehash) [![MyGet Pre Release](https://img.shields.io/myget/coenm/vpre/CoenM.ImageSharp.ImageHash.svg?label=myget)](https://www.myget.org/feed/Packages/coenm/) |

.NET Standard library containing multiple algorithms to calculate perceptual hashes of images and to calculate similarity using those hashes.

## Credits
This project implements the following algorithms:
- AverageHash by Dr. Neal Krawetz. Check his blog/article [here](http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html);
- PerceptualHash also by Dr. Neal Krawetz.
- DifferenceHash by David Oftedal with minor improvements of Dr. Neal Krawetz. More information can be found [here](http://01101001.net/programming.php) or [here](http://www.hackerfactor.com/blog/index.php?/archives/529-Kind-of-Like-That.html).

Misc:
- This project uses the [ImageSharp](https://sixlabors.com/projects/imagesharp/) netstandard library for image processing;
- Also based on existing implementations of already listed algorithms: [jenssegers/imagehash](https://github.com/jenssegers/imagehash) and [jforshee/ImageHashing](https://github.com/jforshee/ImageHashing).

## Goal
I want to keep my photo's organized as much as possible with as less effort as possible. Hashing files is pretty useless to find (almost) duplicate photo's. Updating its meta data, applying a simple filter (instagram has a lot of those), or compressing a photo (sending it using Whatsapp or GMail) results in a different raw file content and therefore for a different hash. Using these algorithms I'm able to find similar images.

## Perceptual hash
Definition by [phash.org](https://www.phash.org/)

> A perceptual hash is a fingerprint of a multimedia file derived from various features from its content. Unlike cryptographic hash functions which rely on the avalanche effect of small changes in input leading to drastic changes in the output, perceptual hashes are "close" to one another if the features are similar.