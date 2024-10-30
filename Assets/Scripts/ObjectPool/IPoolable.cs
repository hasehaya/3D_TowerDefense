public interface IPoolable
{
    void OnObjectReuse(); // オブジェクトが再利用されるときに呼ばれる
    void OnObjectReturn(); // オブジェクトがプールに戻るときに呼ばれる
}
