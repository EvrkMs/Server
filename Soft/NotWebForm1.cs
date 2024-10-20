using System.Net.WebSockets;

namespace Soft
{
    public partial class Form1
    {
        private void Refresh_Click(object sender, EventArgs e)
        {
            LoadInformation();
        }
        private async void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (client != null && client.State == WebSocketState.Open)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закрытие соединения", CancellationToken.None);
                client.Dispose();
            }
        }
    }
}
