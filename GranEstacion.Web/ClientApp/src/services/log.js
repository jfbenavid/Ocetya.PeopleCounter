export const getLogData = (uri) =>
	fetch(uri)
		.then((data) => data.json())
		.then((data) => {
			data.forEach((x) => {
				x.data.forEach((y) => (y[0] = new Date(y[0])));
			});

			return data;
		});
