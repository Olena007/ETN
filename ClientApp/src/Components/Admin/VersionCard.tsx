import { Card, CardActions, CardContent, Typography } from "@mui/material";
import { useState, useEffect } from "react";
import Col from "react-bootstrap/esm/Col";
import { useParams } from "react-router-dom";

export default function VersionCard(){
    const [arr, setArr ] = useState<Array<any>>([]);

    const IdOf = useParams();
    const entries = Object.values(IdOf);    

    useEffect(() => {
        async function fetchData() {
          const response = await fetch(`https://localhost:7052/api/Apk/Get/${entries}`);
          const json = await response.json();
          const ent = Object.values(json);
          setArr(ent);
        }
        fetchData();
      }, []);

      return(
        <div>
          <Col>
                    <Card sx={{ minWidth: "20%" }}>
            <CardContent>
                <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
                Version
                </Typography>
                <Typography variant="h5" component="div">
                {arr[1]}
                </Typography>
                <Typography sx={{ mb: 1.5 }} color="text.secondary">
                {arr[0]}
                </Typography>
                <Typography variant="body2">
                well meaning and kindly.
                <br />
                {'"a benevolent smile"'}
                </Typography>
            </CardContent>
            <CardActions>
            </CardActions>
        </Card>
                    </Col>
    </div>
    )
}