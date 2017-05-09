package jp.or.hills_kobayashi.docot.utility;

import org.springframework.web.client.RestTemplate;
import org.xml.sax.InputSource;

import javax.xml.xpath.XPath;
import javax.xml.xpath.XPathExpressionException;
import javax.xml.xpath.XPathFactory;
import java.io.StringReader;
import java.text.MessageFormat;

public class ReverseGeoCoder {

    public static class Position {

        private String address;

        private String cityCode;

        private String cityName;

        public String getAddress() {
            return address;
        }

        public void setAddress(String address) {
            this.address = address;
        }

        public String getCityCode() {
            return cityCode;
        }

        public void setCityCode(String cityCode) {
            this.cityCode = cityCode;
        }

        public String getCityName() {
            return cityName;
        }

        public void setCityName(String cityName) {
            this.cityName = cityName;
        }
    }

    public static Position getPosition(Double latitude, Double longitude) throws XPathExpressionException {
        Position position = new Position();

        String url = ApplicationUtils.getProperties("yahoo.reverseGeoCoder.url");
        String appid = ApplicationUtils.getProperties("yahoo.reverseGeoCoder.appid");

        RestTemplate restTemplate = new RestTemplate();
        String xml = restTemplate.getForObject(MessageFormat.format(url, latitude, longitude, appid), String.class);

        XPath xpath = XPathFactory.newInstance().newXPath();
        String address = xpath.evaluate("/*[local-name()='YDF']/*[local-name()='Feature']/*[local-name()='Property']/*[local-name()='Address']", new InputSource(new StringReader(xml)));
        String cityCode = xpath.evaluate("/*[local-name()='YDF']/*[local-name()='Feature']/*[local-name()='Property']/*[local-name()='AddressElement'][*[local-name()='Level'][text()='city']]/*[local-name()='Code']", new InputSource(new StringReader(xml)));
        String cityName = xpath.evaluate("/*[local-name()='YDF']/*[local-name()='Feature']/*[local-name()='Property']/*[local-name()='AddressElement'][*[local-name()='Level'][text()='city']]/*[local-name()='Name']", new InputSource(new StringReader(xml)));

        position.setAddress(address);
        position.setCityCode(cityCode);
        position.setCityName(cityName);
        return position;
    }

}
