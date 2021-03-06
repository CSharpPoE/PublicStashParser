﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using PathOfExile.Model.Items;

namespace PathOfExile.Model.Internal
{
    class ItemConstructor : IJsonConstructor
    {
        private IParser<JObject> Parser { get; }
        private IJsonBuilder<UnspecifiedItem> UnspecifiedItemBuilder{ get; }

        private IDictionary<String, IJsonBuilder<Item>> Builders { get; }

        public ItemConstructor()
        {
            Parser = new JsonParser();

            UnspecifiedItemBuilder = new UnspecifiedItemBuilder();
            Builders = new Dictionary<String, IJsonBuilder<Item>>
            {
                ["Currency"] = new CurrencyBuilder(),
                ["Divination"] = new CardBuilder(),
                ["Weapons"] = new WeaponBuilder()
            };
        }
        
        public Item ConstructFrom(JObject obj)
        {
            return Builders.TryGetValue(Parser.Parse(obj), out var builder)
                ? builder.For(obj).Build()
                : UnspecifiedItemBuilder.For(obj).Build();
        }
    }
}
