namespace MessagingSystem
{
	/// <summary>
	/// Interface to describe a global like messaging system
	/// </summary>
	public interface IMessageBroker
	{
		/// <summary>
		/// Add a subscription to a certain message type to be called everytime
		/// that message is sent through the system
		/// </summary>
		/// <param name="subscription">The subscription object containing necessary details to pass on messages</param>
		void Subscribe(ISubscription subscription);
		
		/// <summary>
		/// Removes a subscription from the internal subscription store
		/// </summary>
		/// <param name="subscription">The subscription object reference that was used when subscribing</param>
		void Unsubscribe(ISubscription subscription);
		
		/// <summary>
		/// Publishes a message once and does not cache the message to the message store.
		/// If this message wasn't subscribed to then it will be lost after published
		/// </summary>
		/// <param name="message">The message to be published</param>
		/// <typeparam name="TMessage">The type of message : IMessage</typeparam>
		void PublishSingle<TMessage>(TMessage message) where TMessage : IMessage;
		
		/// <summary>
		/// Publishes a message and caches it in the internal message store
		/// </summary>
		/// <param name="key">The key object used to identify the message</param>
		/// <param name="message">The message that is being published and cached internally</param>
		/// <typeparam name="TKey">The type of key : IMessageKey</typeparam>
		/// <typeparam name="TMessage">The type of message : IMessage</typeparam>
		void PublishPersistant<TKey, TMessage>(TKey key, TMessage message) where TKey : IMessageKey where TMessage : IMessage;

		/// <summary>
		/// Tries to get a message from the internal message store
		/// </summary>
		/// <param name="key">The key object used to identify the message</param>
		/// <param name="message">The message object that was found in the internal message store</param>
		/// <returns>True if the message was found. False if no message was found</returns>
		bool TryGetPersistantMessage(IMessageKey key, out IMessage message);
		
		/// <summary>
		/// Deletes a message that was cached in the internal message store
		/// </summary>
		/// <param name="key">The key object to identify the message</param>
		/// <typeparam name="TKey">The type of key : IMessageKey</typeparam>
		void DeletePersistant<TKey>(TKey key) where TKey : IMessageKey;
	}
}