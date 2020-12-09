namespace Deveel.Smpp {
	/// <summary>
	/// Provides statuses of a PDU
	/// </summary>
	public enum PduStatus {
		/// <summary>
		/// The entire PDU is invalid
		/// </summary>
		Invalid = 0,

		/// <summary>
		/// The header of the PDU is valid
		/// </summary>
		ValidHeader = 1,

		/// <summary>
		/// The body of the PDU is valid
		/// </summary>
		ValidBody = 2,

		/// <summary>
		/// The PDU is valid for both header and body
		/// </summary>
		Valid = ValidHeader | ValidBody
	}
}
