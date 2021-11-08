namespace CoenM.ImageHash.Test.Data
{
    using System;

    using EasyTestFile;
    using EasyTestFileXunit;

    public static class TestData
    {
        private static readonly EasyTestFileSettings _jpgSettings;

        static TestData()
        {
            _jpgSettings = new EasyTestFileSettings();
            _jpgSettings.UseExtension("jpg");
        }

        public static TestFile AlysonHannigan200x200_0 => EasyTestFile.Load(_jpgSettings);

        public static TestFile AlysonHannigan4x4_0 => EasyTestFile.Load(_jpgSettings);

        public static TestFile AlysonHannigan500x500_0 => EasyTestFile.Load(_jpgSettings);

        public static TestFile AlysonHannigan500x500_1 => EasyTestFile.Load(_jpgSettings);

        public static TestFile Github_1 => EasyTestFile.Load(_jpgSettings);

        public static TestFile Github_2 => EasyTestFile.Load(_jpgSettings);

        public static TestFile NotAnImage => EasyTestFile.Load();

        public static TestFile GetByName(string name)
        {
            return name switch {
                "Alyson_Hannigan_500x500_0.jpg" => AlysonHannigan500x500_0,
                "Alyson_Hannigan_500x500_1.jpg" => AlysonHannigan500x500_1,
                "Alyson_Hannigan_200x200_0.jpg" => AlysonHannigan200x200_0,
                "Alyson_Hannigan_4x4_0.jpg" => AlysonHannigan4x4_0,
                "github_1.jpg" => Github_1,
                "github_2.jpg" => Github_2,
                "Not_an_image.txt" => NotAnImage,
                _ => throw new NotImplementedException()
            };
        }
    }
}
