<script lang="ts">
	import { KanjiRpcClient } from "./proto/KanjiServiceClientPb";
	import { KanjiGetListRequest, KanjiGetListResponse } from "./proto/messages/kanji_types_pb";
	import { readStreamAsync } from "./utils/grpcUtils";

	export let name: string;
	let output: string = "none so far";

	const client = new KanjiRpcClient("https://localhost:7076", null, {
		'withCredentials': true
	});
	const req = new KanjiGetListRequest();

	const onClick = async () => {
		const stream = client.getList(req);
		const res = await readStreamAsync<KanjiGetListResponse>(stream)
		console.log(res)
	};
</script>

<main>
	<h1>Hello {name}!</h1>
	<p>
		Visit the <a href="https://svelte.dev/tutorial">Svelte tutorial</a> to learn
		how to build Svelte apps.
	</p>
	<button on:click={onClick}>get response</button>
	<p>{output}</p>
</main>

<style>
	main {
		text-align: center;
		padding: 1em;
		max-width: 240px;
		margin: 0 auto;
	}

	h1 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 4em;
		font-weight: 100;
	}

	@media (min-width: 640px) {
		main {
			max-width: none;
		}
	}
</style>
