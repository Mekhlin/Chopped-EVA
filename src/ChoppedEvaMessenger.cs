using KSP.UI.Screens;

namespace ChoppedEVA
{
    internal static class ChoppedEvaMessenger
    {
        public static void NotifyDeath(ProtoCrewMember kerbal)
        {
            var title = $"{kerbal.name} has died";
            var text = $"{kerbal.name} has died while on EVA.";
            const MessageSystemButton.MessageButtonColor color = MessageSystemButton.MessageButtonColor.RED;
            const MessageSystemButton.ButtonIcons icon = MessageSystemButton.ButtonIcons.FAIL;
            var message = new MessageSystem.Message(title, text, color, icon);

            MessageSystem.Instance.AddMessage(message);
        }
    }
}
