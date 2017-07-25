using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public static class Destiny
    {
        static List<Event> library = new List<Event>();

        public static void LoadEvents()
        {
            Add(EventGroup.SpaceSector, "Прогулива{g:-лся|-лась} по кораблю медленным шагом, раздумывая о бесконечности жизни.", null, EventFilters.StoryCompleted(StoryStage.ShipExploration));
            Add(EventGroup.SpaceSector, "Захотелось проверить, а будет ли работать корабль на биотопливе, но под рукой не нашлось ни одного биологического существа. Кроме меня.", null, EventFilters.StoryCompleted(StoryStage.ShipExploration));
            Add(EventGroup.SpaceSector, "Случайно замети{g:-л|-ла}, как космический патруль проскочил мимо меня. Интересно, это меня они не увидели, или нет?", null, EventFilters.StoryCompleted(StoryStage.ShipExploration));
            Add(EventGroup.SpaceSector, "Почувствовав, как живот урчит от голода, захотелось проверить запасы провианта. Уж лучше бы этого не дела{g:-л|-ла}.", null, EventFilters.StoryCompleted(StoryStage.ShipExploration));
            Add(EventGroup.SpaceSector, "Проходя мимо панели управления кораблем, услыша{g:-л|-ла} странный сигнал из радиоприемника. То ли со мной пытаются выйти на связь другие расы, то ли просто механизм неисправен.", null, EventFilters.StoryCompleted(StoryStage.ShipExploration));
            Add(EventGroup.SpaceSector, "Увиде{g:-л|-ла} за иллюминатором сгорающие метероиды. Помоли{g:-лся|-лась}, чтобы они пролетели мимо.", null, EventFilters.StoryCompleted(StoryStage.ShipExploration));
            Add(EventGroup.SpaceSector, "Иногда мне кажется, что корабль за мной следит. Возможно, стоит навестить врача.", null, EventFilters.StoryCompleted(StoryStage.ShipExploration));
            Add(EventGroup.SpaceSector, "Внезапно просну{g:-лся|-лась} от невыносимой жары. Как выяснилось, мы едва обогнули какую-то звезду. Не знаю, радоваться жизни или плакать от будущих затрат на починку.", null, EventFilters.StoryCompleted(StoryStage.ShipExploration));
            Add(EventGroup.SpaceSector, "Люблю безграничные просторы Вселенной – кто знает, куда в следующий раз пошлет тебя судьба или космический патруль?", null, EventFilters.StoryCompleted(StoryStage.ShipExploration));
            Add(EventGroup.SpaceSector, "От безделья и скуки нача{g:-л|-ла} плевать в потолок. Невесомость – вещь серьезная.", null, EventFilters.StoryCompleted(StoryStage.ShipExploration));
            Add(EventGroup.SpaceSector, "Наш{g:-ел|-ла} {lootItemA} прямо на полу. Вот удача!", Reward.AsLoot(Quality.Legendary, 1), EventFilters.StoryCompleted(StoryStage.ShipExploration));
            Add(EventGroup.SpaceSector, "Несмотря на то, что {lootItemA}, {lootItemB}, {lootItemC} и {lootItemD} я получи{g:-л|-ла} совершенно случайно, я совсем не против!", Reward.AsLoot(Quality.Common, 4), EventFilters.StoryCompleted(StoryStage.ShipExploration));
            Add(EventGroup.SpaceSector, "Исследование корабля", Reward.AsStoryProgress(StoryStage.Completed), EventFilters.StoryRequired(StoryStage.ShipExploration));
            Add(EventGroup.InTravel, "Полет 01");
            Add(EventGroup.InTravel, "Полет 02");
            Add(EventGroup.InTravel, "Полет 03");
            Add(EventGroup.InTravel, "Полет 04");
            Add(EventGroup.InTravel, "Полет 05");
            Add(EventGroup.InTravel, "Полет 06");
            Add(EventGroup.InTravel, "Полет 07");
            Add(EventGroup.InTravel, "Полет 08");
            Add(EventGroup.InTravel, "Полет 09");
            Add(EventGroup.InTravel, "Полет 10");
            Add(EventGroup.Station, "Станция. Отдых.", Reward.AsStamina(50));
            Add(EventGroup.Station, "Станция. Отдых 2.", Reward.AsStamina(50));
            Add(EventGroup.Station, "Станция. Отдых 3.", Reward.AsStamina(50));
            Add(EventGroup.Station, "Станция. Читаем журналы.", Reward.AsCuriosity(50));
            Add(EventGroup.Station, "Станция. Читаем журналы 2.", Reward.AsCuriosity(50));
            Add(EventGroup.Station, "Станция. Читаем журналы 3.", Reward.AsCuriosity(50));
            Add(EventGroup.Planet, "{planetName}. Немного устал{g:|-а}.", Reward.AsStamina(-25));
            Add(EventGroup.Planet, "{planetName}. Очень устал{g:|-а}.", Reward.AsStamina(-50));
            Add(EventGroup.Planet, "{planetName}. Тут скучно.", Reward.AsCuriosity(-25));
            Add(EventGroup.Planet, "{planetName}. Очень скучно.", Reward.AsCuriosity(-50));
            Add(EventGroup.Planet, "{planetName}. Ничего интересного.");
            Add(EventGroup.Planet, "Ничего тут нет.");
            Add(EventSpecial.OnLevelUp, "Ура! Я ещё на один уровень ста{g:-л|-ла} ближе к смерти!");
            AddByLink(EventGroup.SpaceSector, "newPilot", Reward.AsStoryProgress(StoryStage.ShipExploration), EventFilters.StoryRequired(StoryStage.Initial));
            Add(EventSpecial.OnTravelStart, "Поехали! Цель - {planetNameLong}.");
            Add(EventSpecial.OnPlanetArrival, "Вот она, {planetName}! Правда, ничего не видно, стекла запотели немного.");
            ConsoleEx.Log("Successfully created " + library.Count + " native event(s)");
            foreach (string path in Settings.eventsXmlPaths)
            {
                List<EventBuilder> parsedEvents = XmlManager.ParseEvents(path);
                foreach (EventBuilder parsedEvent in parsedEvents)
                {
                    library.Add((Event)parsedEvent);
                }
                if (parsedEvents.Count > 0)
                    ConsoleEx.Log("Successfully parsed " + parsedEvents.Count + " event(s) in " + path.Substring(path.LastIndexOf("/") + 1));
                else
                    ConsoleEx.Log("No valid events found in " + path.Substring(path.LastIndexOf("/") + 1));
            }
        }

        public static void Add(int eventGroup, string message, Reward reward = null, EventFilters filters = null, int chance = Event.defaultChance)
        {
            string link = Localization.Add(message);
            AddByLink(eventGroup, link, reward, filters);
        }
        public static void AddByLink(int eventGroup, string messageLink, Reward reward = null, EventFilters filters = null, int chance = Event.defaultChance)
        {
            if (reward == null) { reward = Reward.none; }
            if (filters == null) { filters = EventFilters.none; }
            Event e = new Event(eventGroup, messageLink, reward, filters, chance);
            library.Add(e);
        }
        public static void Add(EventSpecial specialGroup, string message, Reward reward = null, EventFilters filters = null, int chance = Event.defaultChance)
        {
            string link = Localization.Add(message);
            AddByLink(specialGroup, link, reward, filters);
        }
        public static void AddByLink(EventSpecial specialGroup, string messageLink, Reward reward = null, EventFilters filters = null, int chance = Event.defaultChance)
        {
            if (reward == null) { reward = Reward.none; }
            if (filters == null) { filters = EventFilters.none; }
            Event e = new Event(specialGroup, messageLink, reward, filters, chance);
            library.Add(e);
        }

        /*public static Event FindEvent(Pilot target)
        {
            int eventGroups = EventGroup.None;
            if (target.planet == null) { eventGroups = EventGroup.Space; }
            else { eventGroups = target.planet.eventGroups; }

            return FindEvent(target, eventGroups);
        }*/
        public static Event FindEvent(Pilot target, int eventGroup)
        {
            List<Event> filteredLibrary = new List<Event>();
            foreach (Event e in library)
            {
                if ((eventGroup & e.eventGroup) > 0 && e.filters.IsGood(target) && !target.blockedEvents.Contains(e))
                {
                    for (int i = 0; i < e.chance; i++)
                        filteredLibrary.Add(e);
                }
            }
            if (filteredLibrary.Count == 0)
                return null;
            else
                return filteredLibrary[Random.Get(0, filteredLibrary.Count)];
        }
        public static Event FindEvent(Pilot target, EventSpecial specialGroup)
        {
            List<Event> filteredLibrary = new List<Event>();
            foreach (Event e in library)
            {
                if (specialGroup == e.specialGroup && e.filters.IsGood(target) && !target.blockedEvents.Contains(e))
                {
                    filteredLibrary.Add(e);
                }
            }
            if (filteredLibrary.Count == 0)
                return null;
            else
                return filteredLibrary[Random.Get(0, filteredLibrary.Count)];
        }
        public static void FullReload()
        {
            ConsoleEx.Log("Clearing the event library");
            library.Clear();
            LoadEvents();
        }
    }
}
