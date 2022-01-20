<script lang="ts">
	import { GreeterClient } from "./proto/GreetServiceClientPb";
	import { HelloRequest, HelloReply } from "./proto/messages/greet_types_pb";

	export let name: string;
	let output: string = "none so far";

	const client = new GreeterClient("https://localhost:7076");
	const req = new HelloRequest();
	req.setName("fuck you");

	const onClick = async () => {
		const stream = client.sayManyHellos(req, {});

		const onEnd = () => {
			stream.removeListener("data", onHello);
			stream.removeListener("end", onEnd);
			stream.cancel();
		};

		stream.on("data", onHello);
		stream.on("end", onEnd);

		function onHello(x: HelloReply) {
			console.log(x.getMessage());
		}
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
