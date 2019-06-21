using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;

namespace k8.Template.Engine
{
    public class QuoteSurroundingEventEmitter : ChainedEventEmitter
    {
        public QuoteSurroundingEventEmitter(IEventEmitter nextEmitter) : base(nextEmitter)
        { }

        public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
        {
            if (eventInfo.Source.StaticType == typeof(Object))
                eventInfo.Style = ScalarStyle.SingleQuoted;
            base.Emit(eventInfo, emitter);
        }
    }
}