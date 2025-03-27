namespace LocalMultiplayer.Runtime
{
    public interface IEquipment
    {
        public float Cooldown { get; protected set; }

        public void Update(float deltaTime);
        public void ActionStart();
        public void ActionEnd();
    }
}