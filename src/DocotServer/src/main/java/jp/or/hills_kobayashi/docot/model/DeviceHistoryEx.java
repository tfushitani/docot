package jp.or.hills_kobayashi.docot.model;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import java.sql.Timestamp;

@Entity
public class DeviceHistoryEx {

    @Id
    @GeneratedValue
    private Long historySeq;

    @Column
    private String deviceId;

    @Column
    private Double latitude;

    @Column
    private Double longitude;

    @Column
    private Timestamp updated;

    @Column
    private String address;

    public Long getHistorySeq() {
        return historySeq;
    }

    public void setHistorySeq(Long historySeq) {
        this.historySeq = historySeq;
    }

    public String getDeviceId() {
        return deviceId;
    }

    public void setDeviceId(String deviceId) {
        this.deviceId = deviceId;
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

    public Timestamp getUpdated() {
        return updated;
    }

    public void setUpdated(Timestamp updated) {
        this.updated = updated;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }
}
