﻿using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;

namespace SmtpServer.Protocol
{
    public sealed class StartTlsCommand : SmtpCommand
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">The server options.</param>
        internal StartTlsCommand(ISmtpServerOptions options) : base(options) { }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="context">The execution context to operate on.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task which asynchronously performs the execution.</returns>
        internal override async Task ExecuteAsync(ISmtpSessionContext context, CancellationToken cancellationToken)
        {
            await context.Text.ReplyAsync(SmtpResponse.ServiceReady, cancellationToken);

            var stream = new SslStream(context.Text.GetInnerStream(), true);

            await stream.AuthenticateAsServerAsync(Options.ServerCertificate, false, Options.SupportedSslProtocols, true);

            context.Text = new NetworkTextStream(stream);
        }
    }
}