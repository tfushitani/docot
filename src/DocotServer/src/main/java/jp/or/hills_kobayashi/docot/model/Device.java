package jp.or.hills_kobayashi.docot.model;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import java.sql.Timestamp;

@Entity
public class Device {

    @Id
    private String deviceId;

    @Column
    private String nickname;

    @Column
    private Double latitude;

    @Column
    private Double longitude;

    @Column
    private Timestamp positionUpdated;

    @Column
    private String address;

    @Column
    private String cityCode;

    @Column
    private String cityName;

    public String getDeviceId() {
        return deviceId;
    }

    public void setDeviceId(String deviceId) {
        this.deviceId = deviceId;
    }

    public String getNickname() {
        return nickname;
    }

    public void setNickname(String nickname) {
        this.nickname = nickname;
    }

    public Double getLatitude() {
        return latitude;
    }

    public void setLatitude(Double latitude) {
        this.latitude = latitude;
    }

    public Double getLongitude() {
        return longitude;
    }

    public void setLongitude(Double longitude) {
        this.longitude = longitude;
    }

    public Timestamp getPositionUpdated() {
        return positionUpdated;
    }

    public void setPositionUpdated(Timestamp positionUpdated) {
        this.positionUpdated = positionUpdated;
    }

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
