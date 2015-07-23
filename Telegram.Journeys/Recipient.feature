Feature: Recipient
	In order to keep in touch with the Sender
	As the Recipient
	I want to receive messages sent to me

Scenario: Receive a message
	Given the Sender sends a message
	When I check my received messages
	Then I should receive the message
