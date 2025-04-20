import { Box, Button, Card, CardActions, CardContent, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Row } from "react-bootstrap";
import Col from "react-bootstrap/esm/Col";

interface IVersionCard{
    apkId: string,
    apk_Name: string,
    apkCreator: string,
    apkDate: Date,
    info: string,
    changelog: string
}

interface IApks{
    apks: object
}


export default function VersionsCards(page: number, count: number){
    const [versions, setVersion] = useState<IVersionCard[]>([]);
    let navigate = useNavigate(); 
    const routeChange = () =>{ 
        let path = `newPath`; 
        navigate(path);
    }

    function link(a: string) :string{
        var str = window.location.href = "version/" + a;
        // let path = a; 
        // navigate(path);
        return str;
    }

    useEffect(() => {
        const fetchGetAll = async () => {
            fetch('https://localhost:7052/api/Apk/GetAll', {
                method: 'POST',
                headers: {
                    'Accept': 'application/json, text/plain, */*',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({pagging: {page: page, count: count}})
                }).then(res => res.json())
                .then(res => {
                    setVersion(res.apks);
                });
        }

        fetchGetAll();
    }, [page]);

    return(
        
        <div>
            <Row xs={1} md={3} className="g-4">
                {versions.map((version) => (
                    <Col onClick={() => window.open(link(version.apkId))}>
                    <Card sx={{ minWidth: "20%" }} key={version.apkId}>
            <CardContent>
                <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
                Version
                </Typography>
                <Typography variant="h5" component="div">
                {version.apk_Name}
                </Typography>
                <Typography sx={{ mb: 1.5 }} color="text.secondary">
                {version.apkId}
                </Typography>
                <Typography variant="body2">
                well meaning and kindly.
                <br />
                {'"a benevolent smile"'}
                </Typography>
            </CardContent>
            <CardActions>
                <Button onClick={() => window.open(link(version.apkId))} size="small">See More</Button>
            </CardActions>
        </Card>
                    </Col>
                ))}
    </Row>
        </div>
        
    );
}