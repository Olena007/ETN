import { Pagination, Stack, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import VersionsCards from "./VersionsCards";
import './Admin.css';
import AdminHeader from "./AdminHeader";

interface Props {
    window?: () => Window;
}

export default function Versions(props: Props){
    const [page, setPage] = useState(1);
    let menu;

    const handleChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setPage(value);
    };

    const [totalObjects, setCount] = useState(0);
    useEffect(() => {
        const fetchGetAll = async () => {
            fetch('https://localhost:7052/api/Apk/GetAll', {
                method: 'POST',
                headers: {
                    'Accept': 'application/json, text/plain, */*',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({pagging: {page: 1, count: 100000000}})
                }).then(res => res.json())
                .then(res => {

                    setCount(res.apks.length);
                });
        }

        fetchGetAll();
    }, []);

    menu = (
        <div>
            <Stack spacing={0}>
      <>
      {VersionsCards(page,9)}
      </>
        <Pagination count={Math.ceil(totalObjects / 9)} page={page} siblingCount={0} onChange={handleChange} className="pag"/>
    </Stack>
        </div>
    )
    
    return(
        <div>
          <AdminHeader prop={props} main={menu}></AdminHeader>
        </div>
    );
}