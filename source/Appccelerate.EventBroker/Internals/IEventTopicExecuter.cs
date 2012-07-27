namespace Appccelerate.EventBroker.Internals
{
    using System;

    public interface IEventTopicExecuter
    {
        void Fire(object sender, EventArgs e, IPublication publication); 
    }
}