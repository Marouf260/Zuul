class Guards
{
    // Fields
    // helth
    private int health;
    private Item loot;
    public Guards(int hp, Item loot)
    {
        this.health = hp;
        this.loot = loot;
    }
//  Is de guard aan het leven?
    public bool IsAlive()
    {
        return health > 0;
    }
//  De helth return
    public int GetHealth()
    {
        return health;
    }
//  de Guard take damage
    public void TakeDamage(int amount)
    {
        Math.Max(health -= amount,0);
        // if (health < 0)
        // {
        //     health = 0;
        // }

    }
//  als de guard dood is en is er item nog dan kan je die pakken "Take ";
    public Item Loot()
    {
        Item tempLoot = loot;
        loot = null; 
        return tempLoot;
    }
}