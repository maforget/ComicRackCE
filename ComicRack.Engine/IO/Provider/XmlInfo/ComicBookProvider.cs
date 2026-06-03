namespace cYo.Projects.ComicRack.Engine.IO.Provider.XmlInfo
{
    [XmlInfoFile("ComicBook.xml", 0)]
    public class ComicBookProvider : XmlInfoProvider<ComicBook, ComicBook>
    {
        public override ComicBook ToXml(ComicBook xmlInfo) => xmlInfo;
    }
}
