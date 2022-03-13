using MyNihongo.WebApi.Infrastructure.Kanji;

namespace MyNihongo.Tests.GrpcClient;

public sealed class KanjiClient : KanjiRpc.KanjiRpcClient
{
	public KanjiClient(ChannelBase channel)
		: base(channel)
	{
	}
}