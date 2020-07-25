import endpoints from "../util/endpoints";

export const getCamerasLog = () =>
	fetch(`${endpoints.CAMERAS_LOG}/144000`)
		.then((data) => data.json())
		.then((data) => data);
