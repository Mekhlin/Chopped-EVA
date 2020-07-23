using KSP.UI.Screens;

namespace ChoppedEVA
{
    internal static class ChoppedMessenger
    {
        public static void SendEulogy(ProtoCrewMember kerbal)
        {
            const string title = "Eulogy";
            var text = $"Sadly {kerbal.name} has died while on EVA. {GenderText(kerbal)} will be remembered";
            const MessageSystemButton.MessageButtonColor color = MessageSystemButton.MessageButtonColor.ORANGE;
            const MessageSystemButton.ButtonIcons icon = MessageSystemButton.ButtonIcons.FAIL;
            var message = new MessageSystem.Message(title, text, color, icon);

            MessageSystem.Instance.AddMessage(message);

            
        }

        internal static string GenderText(ProtoCrewMember kerbal) => kerbal.gender == ProtoCrewMember.Gender.Male ? "He" : "She";
    }
}
