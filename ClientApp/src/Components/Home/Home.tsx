import { useEffect, useState } from "react";
import { Pagination, Stack } from "@mui/material";
import CarsCards from "./CarCards";

interface Props {
    window?: () => Window;
}

export default function Home(){
    const [page, setPage] = useState(1);
    let menu;

    const handleChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setPage(value);
    };

    const [totalObjects, setCount] = useState(0);
    useEffect(() => {
        const fetchGetAll = async () => {
            fetch('https://localhost:7001/api/Car/GetAll', {
                method: 'POST',
                headers: {
                    'Accept': 'application/json, text/plain, */*',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({pagging: {page: 1, count: 100000000}})
                }).then(res => res.json())
                .then(res => {

                    setCount(res.cars.length);
                });
        }

        fetchGetAll();
    }, []);
    menu = (
        <div>
            <Stack spacing={0}>
      <>
      {CarsCards(page,6)}
      </>
        <Pagination count={Math.ceil(totalObjects / 6)} page={page} siblingCount={0} onChange={handleChange} className="pag"/>
    </Stack>
        </div>
    );
    return(
        <div>
          {menu}
        </div>
    );
}