using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
namespace RPG
{
    public class Controller
    {
        private World? world;
        private Display_info? display_Info;
        public Controller(World? world)
        {
            if (world != null)
            {
                this.world = world;
            }
            display_Info = Display_info.GetInstance((43, 2));
        }
        public bool Handle(ActionDTO action, int playerID)
        {
            if (action == null) return true ;
            switch (world.Start_Game(world.Players[(char)('0' + playerID)], action.Action))
            {
                case 1:
                    return true;
                case -1:
                    return false;
                default : return true;
            }
        }
        public ActionDTO CreateAction()
        {
            ConsoleKeyInfo keys = Console.ReadKey(true);
            if (keys.Key == ConsoleKey.I || keys.Key == ConsoleKey.P || keys.Key == ConsoleKey.P)
            { ConsoleKeyInfo first = Console.ReadKey(false);

                ConsoleKeyInfo second = Console.ReadKey(true);

               string combinedAction = $"{char.ToUpper(keys.KeyChar)}{char.ToUpper(first.KeyChar)}{char.ToUpper(second.KeyChar)}";
                return new ActionDTO(combinedAction); }
            else {
             return  new ActionDTO(KeyBinds.GetValue(keys.Key));
            }
            
        }
    }
}