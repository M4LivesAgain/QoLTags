using BananaWatch.Pages;
using BepInEx;
using System;

namespace BananaWatch.QoLTags
{
    [BepInPlugin("M4.QoLTags", "QoLTags", "1.0.0")]
    public class QoLTags : BaseUnityPlugin { }

    public class QoLTagsPage : BananaWatchPage
    {
        public override string MMHeader => "<color=#ffff00><i>QoLTags</i></color>";
        public override bool MMDisplay => true;

        private SelectionHandler selectionHandler = new SelectionHandler();

        private bool qoltagtoggle = false;
        private string displayOption = "NAMES + PLATFORMS";

        public override void PageOpened()
        {
            selectionHandler.maxIndex = 1;
            selectionHandler.currentIndex = 0;
        }

        public string SelectionArrow(int index, string text)
            => selectionHandler.currentIndex == index ? $"<color=#FD0000>></color> {text}" : $" {text}";

        public override string RenderScreenContent()
        {
            string header = "<color=#ffff00>==</color> <color=#000000>Q</color><color=#1A1A1A>o</color><color=#333333>L</color><color=#4D4D4D>T</color><color=#666666>a</color><color=#808080>g</color><color=#999999>s</color> <color=#ffff00>==</color>\r\n\r\n";
            header += $"Toggle QoLTags: \r\n <color={(qoltagtoggle ? "green" : "red")}>{(qoltagtoggle ? "Enabled" : "Disabled")}</color>\r\n";
            header += "Display Options: \r\n";
            header += $"< {displayOption} >\r\n";

            return header;
        }

        public override void ButtonPressed(BananaWatchButton buttonType)
        {
            switch (buttonType)
            {
                case BananaWatchButton.Up:
                    selectionHandler.MoveSelectionUp();
                    break;
                case BananaWatchButton.Down:
                    selectionHandler.MoveSelectionDown();
                    break;
                case BananaWatchButton.Enter:
                    HandleSelection();
                    break;
                case BananaWatchButton.Left:
                    if (selectionHandler.currentIndex == 0)
                    {
                        ChangeDisplayOption(-1);
                    }
                    break;
                case BananaWatchButton.Right:
                    if (selectionHandler.currentIndex == 0) 
                    {
                        ChangeDisplayOption(1);
                    }
                    break;
                case BananaWatchButton.Back:
                    NavigateToMM();
                    break;
            }
        }

        private void HandleSelection()
        {
            if (selectionHandler.currentIndex == 0)
            {
                qoltagtoggle = !qoltagtoggle;
                QoLTagHandler.ToggleNametag(qoltagtoggle);
            }
        }

        private void ChangeDisplayOption(int direction)
        {
            string[] options = { "NAMES", "PLATFORMS", "NAMES + PLATFORMS" };
            int currentIndex = Array.IndexOf(options, displayOption);
            int newIndex = (currentIndex + direction + options.Length) % options.Length;
            displayOption = options[newIndex];

            // Toggle corresponding features based on the display option
            if (displayOption == "NAMES")
            {
                QoLTagHandler.ToggleNames(true);
                QoLTagHandler.TogglePlatform(false);
            }
            else if (displayOption == "PLATFORMS")
            {
                QoLTagHandler.ToggleNames(false);
                QoLTagHandler.TogglePlatform(true);
            }
            else if (displayOption == "NAMES + PLATFORMS")
            {
                QoLTagHandler.ToggleNames(true);
                QoLTagHandler.TogglePlatform(true);
            }
        }
    }

    public static class QoLTagHandler
    {
        public static void ToggleNametag(bool enabled)
        {
            if (enabled)
            {
                QoLTag.ToggleNametag(true);
            }
            else
            {
                QoLTag.ToggleNametag(false);
            }
        }

        public static void TogglePlatform(bool enabled)
        {
            if (enabled)
            {
                QoLTag.TogglePlatform(true);
            }
            else
            {
                QoLTag.TogglePlatform(false);
            }
        }

        public static void ToggleNames(bool enabled)
        {
            if (enabled)
            {
                QoLTag.ToggleNames(true);
            }
            else
            {
                QoLTag.ToggleNames(false);
            }
        }
    }
}
