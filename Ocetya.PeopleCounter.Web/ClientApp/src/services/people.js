import endpoints from "../util/endpoints";

export const getPeopleCount = () =>
	fetch(endpoints.PEOPLE_COUNT).then((data) => data.json());
