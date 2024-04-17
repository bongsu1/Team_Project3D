public interface IDamagable
{
    /// <summary>
    /// damage에 받을 데미지 입력
    /// ex) hp -= damage;
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage);
}
