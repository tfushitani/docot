package jp.or.hills_kobayashi.docot.repository;

import jp.or.hills_kobayashi.docot.model.Device;
import jp.or.hills_kobayashi.docot.model.DeviceHistory;
import jp.or.hills_kobayashi.docot.model.DeviceHistoryEx;
import org.springframework.data.jpa.repository.JpaRepository;

import java.sql.Timestamp;
import java.util.List;

public interface DeviceHistoryExRepository extends JpaRepository<DeviceHistoryEx, Long> {

    List<DeviceHistoryEx> findByDeviceIdAndUpdatedGreaterThanEqualOrderByUpdatedDesc(String deviceId, Timestamp update);

    List<DeviceHistoryEx> findByDeviceIdOrderByUpdatedDesc(String deviceId);

}
