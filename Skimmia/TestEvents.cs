namespace Skimmia
{
    public class TestEvents
    {
        public readonly EventStream<Test> RootStarted = new EventStream<Test>();
        public readonly EventStream<Test> RootComplete = new EventStream<Test>();

        public readonly EventStream<Test> NodeFound = new EventStream<Test>();
        public readonly EventStream<Test> NodeEntered = new EventStream<Test>();
        public readonly EventStream<Test> NodeExited = new EventStream<Test>();

        public readonly EventStream<Test> LeafComplete = new EventStream<Test>();
    }
}