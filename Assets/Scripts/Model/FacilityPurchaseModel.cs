using UnityEngine;

public class FacilityPurchaseModel
{
    public Sprite Icon { get; private set; }
    public int Index { get; private set; }
    public int Price { get; private set; }
    public FacilityParameter Parameter { get; private set; }

    public FacilityPurchaseModel(int index, FacilityParameter parameter)
    {
        Index = index;
        Parameter = parameter;
        Icon = parameter.icon;
        Price = parameter.price;
    }
}
