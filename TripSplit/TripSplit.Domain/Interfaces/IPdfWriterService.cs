namespace TripSplit.Domain.Interfaces
{
    public interface IPdfWriterService
    {
        Task<string> WritePdf(int tripId);
    }
}