using System.Linq;
using System.Text.RegularExpressions;

namespace Content.Server.Chat.Systems;

public sealed partial class ChatSystem
{
    private static readonly Dictionary<string, string> SlangReplace = new()
    {
        // Game
        { "хос", "ГСБ" },
        { "хосу", "ГСБ" },
        { "хоса", "ГСБ" },
        { "хосом", "ГСБ" },
        { "смо", "ГВ" },
        { "се", "СИ" },
        { "хоп", "ГП" },
        { "хопу", "ГП" },
        { "хопа", "ГП" },
        { "хопом", "ГП" },
        { "рд", "НР" },
        { "вард", "смотритель" },
        { "варду", "смотрителю" },
        { "варда", "смотрителя" },
        { "вардом", "смотрителем" },
        { "варден", "смотритель" },
        { "вардену", "смотрителю" },
        { "вардена", "смотрителя" },
        { "варденом", "смотрителем" },
        { "бригмед", "бригмедик" },
        { "бригмеду", "бригмедику" },
        { "бригмеда", "бригмедика" },
        { "бригмедом", "бригмедиком" },
        { "парамед", "парамедик" },
        { "парамеду", "парамедику" },
        { "парамеда", "парамедика" },
        { "парамедом", "парамедиком" },
        { "кэп", "капитан" },
        { "кеп", "капитан" },
        { "кэпу", "капитану" },
        { "кепу", "капитану" },
        { "кэпа", "капитана" },
        { "кепа", "капитана" },
        { "кэпом", "капитаном" },
        { "кепом", "капитаном" },
        { "геник", "генератор" },
        { "гравген", "генератор гравитации" },
        { "гравиген", "генератор гравитации" },
        { "кк", "красный код" },
        { "ск", "синий код" },
        { "зк", "зелёный код" },
        { "пда", "КПК" },
        { "корп", "корпоративный" },
        { "дэк", "детектив" },
        { "дэку", "детективу" },
        { "дэка", "детектива" },
        { "дэком", "детективом" },
        { "дек", "детектив" },
        { "деку", "детективу" },
        { "дека", "детектива" },
        { "деком", "детективом" },
        { "мш", "имплант защиты разума" },
        { "трейтор", "предатель" },
        { "инжи", "инженеры" },
        { "инжы", "инженеры" },
        { "инжам", "инженерам" },
        { "инжей", "инженеров" },
        { "инжами", "инженерами" },
        { "инж", "инженер" },
        { "инжу", "инженеру" },
        { "инжа", "инженера" },
        { "инжом", "инженером" },
        { "инжинер", "инженер" },
        { "инжинеру", "инженеру" },
        { "инжинера", "инженера" },
        { "инжинером", "инженером" },
        { "утиль", "утилизатор" },
        { "утилю", "утилизатору" },
        { "утиля", "утилизатора" },
        { "утилем", "утилизатором" },
        { "утили", "утилизаторы" },
        { "утилям", "утилизаторам" },
        { "утилей", "утилизаторов" },
        { "утилями", "утилизаторами" },
        { "яо", "ядерные оперативники" }, // nobraindead?
        { "нюк", "ядерный оперативник" },
        { "нюкеры", "ядерные оперативники" },
        { "нюкер", "ядерный оперативник" },
        { "нюкеровец", "ядерный оперативник" },
        { "рева", "ревенант" },
        { "ревы", "ревенанта" },
        { "реву", "ревенанта" },
        { "реве", "ревенанту" },
        { "аирлок", "шлюз" },
        { "аирлоки", "шлюзы" },
        { "айрлок", "шлюз" },
        { "айрлоки", "шлюзы" },
        { "визард", "волшебник" },
        { "дизарм", "толкнуть" },
        { "синга", "сингулярность" },
        { "сингу", "сингулярность" },
        { "синги", "сингулярности" },
        { "соляры", "солнечные панели" },
        { "соляр", "солнечных панелей" },
        { "разгерм", "разгерметизация" },
        { "арт", "артефакт" },
        { "арту", "артефакту" },
        { "арта", "артефакта" },
        { "артом", "артефактом" },
        { "арты", "артефакты" },
        { "артам", "артефактам" },
        { "артов", "артефактов" },
        { "артами", "артефактами" },
        { "бикардин", "бикаридин" },
        { "бик", "бикаридин" },
        { "бика", "бикаридин" },
        { "бику", "бикаридин" },
        { "бики", "бикаридина" },
        { "бикой", "бикаридином" },
        { "декс", "дексалин" },
        { "декса", "дексалин" },
        { "дексу", "дексалин" },
        { "дерм", "дермалин" },
        { "дерма", "дермалин" },
        { "дерму", "дермалин" },
        { "дермы", "дермалина" },
        { "дерми", "дермалина" },
        { "дермой", "дермалином" },
        { "дил", "диловен" },
        { "дило", "диловен" },
        { "дилом", "диловеном" },
        { "хиро", "хироналин" },
        { "космо", "космоциллин" },
        { "трик", "трикордразин" },
        { "эпи", "эпинефрин" },
        { "эпин", "эпинефрин" },
        { "фаланг", "фалангимин" },
        { "фалан", "фалангимин" },
        { "передоз", "передозировка" },
        // IC
        { "норм", "нормально" },
        { "хз", "не знаю" },
        { "синд", "синдикат" },
        { "пон", "понятно" },
        { "непон", "не понятно" },
        { "нипон", "не понятно" },
        { "кста", "кстати" },
        { "кст", "кстати" },
        { "плз", "пожалуйста" },
        { "пж", "пожалуйста" },
        { "спс", "спасибо" },
        { "сяб", "спасибо" },
        { "прив", "привет" },
        { "ок", "окей" },
        { "чел", "мужик" },
        { "лан", "ладно" },
        { "збс", "заебись" },
        { "мб", "может быть" },
        { "оч", "очень" },
        { "омг", "боже мой" },
        { "нзч", "не за что" },
        { "пок", "пока" },
        { "бб", "пока" },
        { "пох", "плевать" },
        { "ясн", "ясно" },
        { "всм", "всмысле" },
        { "чзх", "что за херня?" },
        { "изи", "легко" },
        { "гг", "хорошо сработано" },
        { "пруф", "доказательство" },
        { "пруфы", "доказательства" },
        { "пруфи", "доказательства" },
        { "пруфани", "докажи" },
        { "пруфанул", "доказал" },
        { "брух", "мда..." },
        { "имба", "нечестно" },
        { "разлокать", "разблокировать" },
        { "юзать", "использовать" },
        { "юзай", "используй" },
        { "юзнул", "использовал" },
        { "хилл", "лечение" },
        { "подхиль", "полечи" },
        { "хильни", "полечи" },
        { "хелп", "помоги" },
        { "хелпани", "помоги" },
        { "хелпанул", "помог" },
        { "рофл", "прикол" },
        { "рофлишь", "шутишь" },
        // OOC
        { "афк", "ссд" },
        { "админ", "бог" },
        { "админы", "боги" },
        { "админов", "богов" },
        { "забанят", "покарают" },
        { "бан", "наказание" },
        { "пермач", "наказание" },
        { "перм", "наказание" },
        { "запермили", "наказали" },
        { "запермят", "накажут" },
        { "нонрп", "плохо" },
        { "нрп", "плохо" },
        { "рдм", "плохо" },
        { "дм", "плохо" },
        { "гриф", "плохо" },
        { "фрикил", "плохо" },
        { "фрикилл", "плохо" },
        { "лкм", "левая рука" },
        { "пкм", "правая рука" },
    };

    private string ReplaceWords(string message)
    {
        if (string.IsNullOrEmpty(message))
            return message;

        return Regex.Replace(message, "\\b(\\w+)\\b", match =>
        {
            bool isUpperCase = match.Value.All(Char.IsUpper);

            if (SlangReplace.TryGetValue(match.Value.ToLower(), out var replacement))
                return isUpperCase ? replacement.ToUpper() : replacement;
            return match.Value;
        });
    }
}
