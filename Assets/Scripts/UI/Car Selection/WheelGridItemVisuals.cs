namespace UI.Car_Selection
{
    public class WheelGridItemVisuals : GridItemVisuals
    {
        public override void Bind(GridData data)
        {
            if (data is WheelData data2)
            {
                if (data2.ID < 0)
                {
                    ContentRoot.gameObject.SetActive(false);
                }
                else
                {
                    ContentRoot.gameObject.SetActive(true);
                }
                IconBlock.SetImage(data2.WheelStatics.WheelSprite);
                IsLocked = data2.IsLocked;
            }
        }
    }
    public class PaintGridItemVisuals : GridItemVisuals
    {

        public override void Bind(GridData data)
        {
            if (data is PaintData data2)
            {
                if (data2.ID < 0)
                {
                    ContentRoot.gameObject.SetActive(false);
                }
                else
                {
                    ContentRoot.gameObject.SetActive(true);
                }
                IconBlock.Color = data2.Color;
                IsLocked = data2.IsLocked;
            }
        }
    }
}