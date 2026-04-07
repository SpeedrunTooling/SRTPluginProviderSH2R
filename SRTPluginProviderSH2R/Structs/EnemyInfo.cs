namespace SRTPluginProviderSH2R.Structs;

public class EnemyInfo
{
    internal float hp;
    public float HP => hp;

    internal bool isValid;
    public bool IsValid => isValid;

    public bool IsAlive => HP > 0f;
}
