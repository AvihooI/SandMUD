namespace SandModel
{
    //Effectively Done
    public enum ItemCategory
    {
        WearHead, //e.g. hat
        WearFace, //e.g. mask
        WearHeadFace, //e.g. helmet
        WearNeck, //e.g. scarf
        WearBody, //e.g. shirt
        WearTop, //e.g. jacket
        WearArm, //e.g. sleeve
        WearHand, //e.g. glove/gauntlet
        WearWrist, //e.g. bracelet
        WearFinger, //e.g ring
        WearBottom, //e.g. pants
        HeldOneHanded, //e.g. knife/shield
        HeldTwoHanded, //e.g. two-handed sword
        EdibleEat, //e.g. bread
        EdibleDrink, //e.g. water
        Other //Special logic involved
    }

    //DONE
    public class TemplateItem //This defines a template
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public bool Stationary { get; set; } //Indicates if the item can be carried by a character or not 
        public ItemCategory Type { get; set; }
        public string Description { get; set; }
        public string StatsJson { get; set; }
    }
}