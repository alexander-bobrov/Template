using Database;
using Microsoft.EntityFrameworkCore;
using SmtpServer;
using SmtpServer.Protocol;
using SmtpServer.Storage;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Template.Services.MailService
{
    public class MessageStorage : MessageStore
    {
        private readonly IDbContextFactory<TemplateContext> dbFactory;

        public MessageStorage(IDbContextFactory<TemplateContext> dbFactory)
        {
            this.dbFactory = dbFactory;
        }
        public override async Task<SmtpResponse> SaveAsync(ISessionContext context, IMessageTransaction transaction, ReadOnlySequence<byte> buffer, CancellationToken cancellationToken)
        {
            await using var stream = new MemoryStream();

            var position = buffer.GetPosition(0);
            while (buffer.TryGet(ref position, out var memory))
            {
                await stream.WriteAsync(memory, cancellationToken);
            }

            stream.Position = 0;
            var message = await MimeKit.MimeMessage.LoadAsync(stream, cancellationToken);

            using (var db = dbFactory.CreateDbContext())
            {
                db.Messages.Add(new Database.Entities.MessageEntity
                {
                    From = message.From.ToString(),
                    To = message.To.ToString(),
                    Text = message.TextBody,
                    Html = message.HtmlBody

                });

                await db.SaveChangesAsync(cancellationToken);
            }

            return SmtpResponse.Ok;
        }

    }
}
