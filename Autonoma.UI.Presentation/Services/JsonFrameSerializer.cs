using Autonoma.UI.Presentation.Abstractions;
using Autonoma.UI.Presentation.Infrastructure;
using Autonoma.UI.Presentation.ViewModels;
using Newtonsoft.Json;
using System;

namespace Autonoma.UI.Presentation.Services
{
    public class JsonFrameSerializer : IFrameSerializer
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };

        public string SerializeFrame(IFrame model)
        {
            return Serialize(model);
        }

        public string SerializeElement(IElement model)
        {
            return Serialize(model);
        }

        public FrameViewModel DeserializeFrame(string data)
        {
            var frame = Deserialize<FrameViewModel>(data);
            if (frame == null)
                throw new InvalidOperationException();

            return frame;
        }

        public ElementViewModel DeserializeElement(string data)
        {
            var element = Deserialize<ElementViewModel>(data);
            if (element == null)
                throw new InvalidOperationException();

            return element;
        }

        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }

        public T? Deserialize<T>(string data)
        {
            var result = JsonConvert.DeserializeObject<T>(data, _settings);
            return result;
        }
    }
}
