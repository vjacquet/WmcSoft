namespace WmcSoft.Windows.Forms
{
    /// <summary>
    /// Blend style.
    /// </summary>
    [RM.Description("BlendStyle")]
    public enum BlendStyle
    {
        [RM.Description("BlendStyle.Horizontal")]
        Horizontal = 0,

        [RM.Description("BlendStyle.Vertical")]
        Vertical,

        [RM.Description("BlendStyle.ForwardDiagonal")]
        ForwardDiagonal,

        [RM.Description("BlendStyle.BackwardDiagonal")]
        BackwardDiagonal
    };

}
