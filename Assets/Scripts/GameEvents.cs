public static class GameEventSystem
{
    public delegate void NewOrderEvent(Order order);
    public static NewOrderEvent OnNewOrderEvent;
}