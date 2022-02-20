import type { ClientReadableStream } from "grpc-web";

export function readStreamAsync<T>(stream: ClientReadableStream<unknown>): Promise<T[]> {
	let res: T[] = []

	const onData = (x: T) => {
		res.push(x)
	}

	return new Promise<T[]>(resolve => {
		const onEnd = () => {
			stream.removeListener("data", onData)
			stream.removeListener("end", onEnd)
			stream.cancel()
			resolve(res)
		}

		stream.on("data", onData)
		stream.on("end", onEnd)
	})
}