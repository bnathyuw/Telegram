using TechTalk.SpecFlow;

namespace Telegram.Journeys
{
    [Binding]
    public class RecipientSteps
    {
        private readonly Sender _sender;
        private readonly Recipient _recipient;

        public RecipientSteps(Sender sender, Recipient recipient)
        {
            _sender = sender;
            _recipient = recipient;
        }

        [Given(@"the Sender sends a message")]
        public void GivenTheSenderSendsAMessage()
        {
            _sender.Send("message");
        }

        [When(@"I check my received messages")]
        public void WhenICheckMyReceivedMessages()
        {
            _recipient.WaitforMessage();
        }

        [Then(@"I should receive the message")]
        public void ThenIShouldReceiveTheMessage()
        {
            _recipient.AssertMessageMatches("message");
        }
    }
}
